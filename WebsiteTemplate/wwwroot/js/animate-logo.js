/* Logo anim from mjau-mjau.com http://benjii.me/js/animate-logo.js */
/* Requires jquery.js and velocity.js 

    <div class="sweetas-logo">
        <span class="m1 big" style="color: rgb(236, 100, 75); transform: rotateZ(0deg); display:inline-block">Ben</span>
        <span class="m2 big" style="color: rgb(190, 195, 201); transform: rotateZ(0deg); display:inline-block">Cull'sxsdaf sadfsadf sadf sadf sadf sads</span>
        <span class="m3 big" style="color: rgb(240, 150, 9); transform: rotateZ(0deg); display:inline-block">Blog</span>
    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/velocity/1.4.0/velocity.js"></script>
    <script src="/js/animate-logo.js"></script>
    
*/
(function ($) {

    // Animate logo on hover

    // vars
    var logo = $(".sweetas-logo");
    var m1 = $(".sweetas-logo .m1");
    var m2 = $(".sweetas-logo .m2");
    var m3 = $(".sweetas-logo .m3");
    var easings = ["easeOutQuad", "easeInOutQuad", "easeInOutBack", "easeOutElastic", "easeOutBounce"];
    var values = [[20, 180, 0], [170, 170, 0], [20, 360, 0], [350, 0, 0], [0, 40, 360], [0, 320, 0], [0, 180, 0], [180, 180, 0]];

    m1.colh = [100, 110, 120];
    m2.colh = [236, 100, 75];
    m3.colh = [240, 150, 9];

    // logo hover
    logo.hover(function () {
        m1.logoanim(1);
        m2.logoanim(2);
        m3.logoanim(3);
    }, function () {
        m1.velocity("reverse");
        m2.velocity("reverse");
        m3.velocity("reverse");
    });

    // logo anim prototype
    $.fn.logoanim = function (item) {

        // duration
        var duration = Math.round(Math.random() * 400) + 200;

        // anim object
        var a = Math.floor(Math.random() * values.length);

        // easing
        var e = Math.floor(Math.random() * easings.length);
        var easing = easings[e];
        if (e >= 2) { duration *= 2 }

        // velocity
        $(this).velocity({
            rotateX: values[a][0] * (Math.round(Math.random()) * 2 - 1),
            rotateY: values[a][1] * (Math.round(Math.random()) * 2 - 1),
            rotateZ: values[a][2] * (Math.round(Math.random()) * 2 - 1),
            colorRed: this.colh[0],
            colorGreen: this.colh[1],
            colorBlue: this.colh[2]
        }, {
            duration: duration,
            easing: easing
        });
    }

    // animate logo on document load
    $(document).ready(function () {
        m1.logoanim(1);
        m1.velocity("reverse");
        m2.logoanim(2);
        m2.velocity("reverse");
        m3.logoanim(3);
        m3.velocity("reverse");
    });

})(jQuery);