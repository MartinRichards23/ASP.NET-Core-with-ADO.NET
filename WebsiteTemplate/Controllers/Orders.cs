/*using WebsiteTemplate.Core;
using WebsiteTemplate.Core.Data;
using WebsiteTemplate.Core.Emailing;
using WebsiteTemplate.Models;
using WebsiteTemplate.Extensions;
using WebsiteTemplate.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemPlus;
using SystemPlus.Threading;

namespace WebsiteTemplate.Controllers
{
    [Authorize]
    public class Orders : MyBaseController
    {
        #region Fields

        readonly Emailer emailer;

        #endregion

        public Orders(UserManager<User> userManager, DataAccess dataAccess, Emailer emailer)
            : base(userManager, dataAccess)
        {
            this.emailer = emailer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(ChooseAccountType));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Pricing()
        {
            User user = await GetUserAsync();
            this.SetPricingViewBag(user);
            return View();
        }

        #region Orders

        [HttpGet]
        public async Task<IActionResult> OrderHistory()
        {
            User user = await GetUserAsync();
            IList<Order> orders = Database.GetOrdersForUser(user.Id).Where(o => o.Status == Core.Models.OrderStatus.Complete).ToList();
            IList<PurchaseTransaction> transactions = Database.GetTransactionsForUser(user.Id, 50);

            OrderHistoryModel model = new OrderHistoryModel()
            {
                Orders = orders,
                Transactions = transactions,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            User user = await GetUserAsync();

            Database.DeleteOrder(user.Id, orderId);

            return RedirectToAction(nameof(OrderHistory));
        }

        #endregion

        #region Payment methods

        [HttpGet]
        public async Task<IActionResult> ManagePaymentMethods()
        {
            User user = await GetUserAsync();
            ViewBag.PaymentMethods = Database.GetPaymentMethodsForUser(user.Id);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfigureAutoPayment(int paymentId)
        {
            User user = await GetUserAsync();
            PaymentMethod method = Database.GetPaymentMethod(user.Id, paymentId);

            ViewBag.PaymentMethod = method;

            string choice;
            int fixedAmount = 0;
            fixedAmount = MathTools.Clip(fixedAmount, 0, 10000);

            if (method.PaymentMode == PaymentMode.VariableMonthly)
                choice = "auto";
            else if (method.PaymentMode == PaymentMode.FixedMonthly)
                choice = "fixed";
            else
                choice = "none";

            ConfigAutoPaymentModel model = new ConfigAutoPaymentModel()
            {
                PaymentMethodId = method.Id,
                PaymentMethodName = method.FriendlyName(),
                Choice = choice,
            };

            this.SetPricingViewBag(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfigureAutoPayment(ConfigAutoPaymentModel model)
        {
            User user = await GetUserAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.PaymentMethods = Database.GetPaymentMethodsForUser(user.Id);
                this.SetPricingViewBag(user);
                return View(nameof(ConfigureAutoPayment), model);
            }

            //if (model.Choice == "fixed")
            //{
            //    ModelState.AddModelError(string.Empty, string.Format("Minimum amount of  is {0}", 0));
            //    ViewBag.PaymentMethods = Database.GetPaymentMethodsForUser(user.Id);
            //    this.SetPricingViewBag(user);
            //    return View(nameof(ConfigureAutoPayment), model);
            //}

            PaymentMethod method = Database.GetPaymentMethod(user.Id, model.PaymentMethodId);

            DateTime next;
            if (method.LastAutoPaymentTime.HasValue)
                next = method.LastAutoPaymentTime.Value.AddMonths(1);
            else
                next = DateTime.UtcNow + TimeSpan.FromHours(2);
            method.NextAutoPaymentTime = next;

            if (model.Choice == "auto")
            {
                method.PaymentMode = PaymentMode.VariableMonthly;
            }
            else if (model.Choice == "fixed")
            {
                method.PaymentMode = PaymentMode.FixedMonthly;
            }
            else
            {
                method.PaymentMode = PaymentMode.None;
            }

            Database.SetAutoPaymentMethod(method);

            if (method.PaymentMode != PaymentMode.None)
            {
                // send email to say monthly purchase is enabled.
                emailer.SendAutoPaymentSetUpAsync(user, method).DoNotAwait();
            }

            return RedirectToAction(nameof(ManagePaymentMethods));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCardDetails(NewCardModel model)
        {
            User user = await GetUserAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.PaymentMethods = Database.GetPaymentMethodsForUser(user.Id);
                return View(nameof(ManagePaymentMethods), model);
            }

            PaymentMethod method = CreatePaymentMethod(user.Id, model);

            return RedirectToAction(nameof(ManagePaymentMethods));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePaymentMethod(int paymentId)
        {
            User user = await GetUserAsync();
            PaymentMethod method = Database.GetPaymentMethod(user.Id, paymentId);

            if (method.PaymentType == PaymentType.Card)
            {
                WorldpayRestClient restClient = PaymentTools.MakeClient(method.CardDetails.IsDemo);

                // delete it from worldpay
                restClient.GetTokenService().Delete(method.CardDetails.Token);

                // delete from the database
                Database.DeletePaymentMethod(user.Id, method.Id);
            }
            else
            {
                throw new NotSupportedException();
            }

            return RedirectToAction(nameof(ManagePaymentMethods));
        }

        #endregion

        #region One off top-up
        
        [HttpGet]
        public async Task<IActionResult> SinglePurchase()
        {
            User user = await GetUserAsync();

            CreateOrderModel model = new CreateOrderModel()
            {

            };

            this.SetPricingViewBag(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SinglePurchase(CreateOrderModel model)
        {
            User user = await GetUserAsync();

            if (!ModelState.IsValid)
            {
                this.SetPricingViewBag(user);
                return View(nameof(SinglePurchase), model);
            }
            
            //if ()
            //{
            //    this.SetPricingViewBag(user);
            //    return View(nameof(SinglePurchase), model);
            //}

            throw new Exception();
            int cost = 0; // cost in pence
            cost = PaymentTools.ConvertCurrency(cost, user.Config.CurrencyInfo.ExchangeRate);

            Order order = new Order(user.Id, user.Config.Currency)
            {
                PaymentMethodId = null,
                Status = Core.Models.OrderStatus.Created,
                Value = cost,
            };

            Database.AddOrder(order);

            return RedirectToAction(nameof(ChoosePaymentMethod), new { orderId = order.Id });
        }


        [HttpGet]
        public async Task<IActionResult> ChooseAccountType()
        {
            User user = await GetUserAsync();

            AccountTypeModel model = new AccountTypeModel()
            {
                Choice = UserLevel.Free.ToString()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChooseAccountType(AccountTypeModel model)
        {
            User user = await GetUserAsync();

            if (!Enum.TryParse(model.Choice, out UserLevel level))
                throw new Exception("unknown level " + model.Choice);

            // todo: check payment method
            
            user.DesiredLevel = level;

            Database.UpdateUser(user);
            
            if(user.PaymentMethodId.HasValue)
            {
                return RedirectToAction(nameof(Manage.Settings), nameof(Manage), new { area = "" });
            }
            else
            {
                return RedirectToAction(nameof(Orders.ManagePaymentMethods), nameof(Orders), new { area = "" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChoosePaymentMethod(int orderId)
        {
            User user = await GetUserAsync();
            Order order = Database.GetOrder(user.Id, orderId);

            ViewBag.OrderId = order.Id;
            ViewBag.PaymentMethods = Database.GetPaymentMethodsForUser(user.Id);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChoosePaymentMethod(int orderId, int paymentMethodId)
        {
            User user = await GetUserAsync();
            Order order = Database.GetOrder(user.Id, orderId);
            PaymentMethod method = Database.GetPaymentMethod(user.Id, paymentMethodId);

            order.PaymentMethodId = method.Id;
            Database.UpdateOrder(order);

            return RedirectToAction(nameof(ConfirmOrder), new { orderId = order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChoosePaymentMethodNew(int orderId, NewCardModel model)
        {
            User user = await GetUserAsync();
            Order order = Database.GetOrder(user.Id, orderId);

            if (!ModelState.IsValid)
            {
                ViewBag.OrderId = order.Id;
                ViewBag.PaymentMethods = Database.GetPaymentMethodsForUser(user.Id);
                return View(nameof(ChoosePaymentMethod), model);
            }

            PaymentMethod method = CreatePaymentMethod(user.Id, model);

            order.PaymentMethodId = method.Id;
            Database.UpdateOrder(order);

            return RedirectToAction(nameof(ConfirmOrder), new { orderId = order.Id });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            User user = await GetUserAsync();
            Order order = Database.GetOrder(user.Id, orderId);
            PaymentMethod method = Database.GetPaymentMethod(user.Id, order.PaymentMethodId.Value);

            ConfirmOrderModel model = new ConfirmOrderModel()
            {
                Order = order,
                PaymentMethod = method,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            User user = await GetUserAsync();
            Order order = Database.GetOrder(user.Id, orderId);
            PaymentMethod method = Database.GetPaymentMethod(user.Id, order.PaymentMethodId.Value);
            var address = Database.GetAddress(method.AddressId);

            if (order.Status == Core.Models.OrderStatus.Complete)
            {
                // don't let order accidentally be processed twice
                throw new Exception(string.Format("Order {0} has already been completed", orderId));
            }
            
            int costInPence = order.Value;

            WorldpayRestClient restClient = PaymentTools.MakeClient(method.CardDetails.IsDemo);
            OrderRequest orderRequest = PaymentTools.MakeOrderRequest(order.Id, user, order, method, address, costInPence);

            try
            {
                OrderResponse orderResponse = restClient.GetOrderService().Create(orderRequest);

                if (orderResponse.paymentStatus != Worldpay.Sdk.Enums.OrderStatus.SUCCESS)
                    throw new Exception(orderResponse.paymentStatusReason);

                order.Status = Core.Models.OrderStatus.Complete;
                order.Code = orderResponse.orderCode;

                Database.AddTransaction(order.UserId, order.Id, order.Value, null);
                Database.UpdateOrder(order);
            }
            catch (WorldpayException ex)
            {
                return this.RedirectToError(ex.Message);
            }

            await emailer.SendPurchaseMadeAsync(user, order, method);

            string message = string.Format("Order id {0} has been completed, at a value of {1} {2:0.00}", order.Id, order.Currency, order.ValueInCurrency);
            return Message(new MessageModel() { Title = "Thank you", Text = message });
        }

        #endregion

        #region Helpers

        private PaymentMethod CreatePaymentMethod(int userId, NewCardModel model)
        {
            bool isDemo = false;

#if DEBUG
            isDemo = true;
#endif

            WorldpayRestClient restClient = PaymentTools.MakeClient(isDemo);

            TokenResponse tokenResponse = restClient.GetTokenService().Get(model.Token);

            StreetAddress address = new StreetAddress()
            {
                UserId = userId,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Address3 = model.Address3,
                City = model.City,
                State = model.State,
                Country = model.Country,
                PostCode = model.PostCode,
            };

            Database.AddAddress(address);

            CardDetails details = new CardDetails()
            {
                Token = tokenResponse.token,
                Reusable = tokenResponse.reusable,
                MaskedNumber = tokenResponse.paymentMethod.maskedCardNumber,
                CardType = tokenResponse.paymentMethod.cardType,
                Name = tokenResponse.paymentMethod.name,
                ExpiryMonth = tokenResponse.paymentMethod.expiryMonth,
                ExpiryYear = tokenResponse.paymentMethod.expiryYear,
                IsDemo = isDemo,
            };

            PaymentMethod method = new PaymentMethod()
            {
                UserId = userId,
                PaymentType = PaymentType.Card,
                CardDetails = details,
                AddressId = address.Id,
            };

            Database.AddPaymentMethod(method);

            return method;
        }

        #endregion
    }
}
*/