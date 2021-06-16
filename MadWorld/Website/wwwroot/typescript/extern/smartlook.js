var SmartLook;
(function (SmartLook) {
    var SmartLookAnalyser = /** @class */ (function () {
        function SmartLookAnalyser() {
        }
        SmartLookAnalyser.prototype.Init = function () {
            window.smartlook || (function (d) {
                var o = window.smartlook = function () { o.api.push(arguments); }, h = d.getElementsByTagName('head')[0];
                var c = d.createElement('script');
                o.api = new Array();
                c.async = true;
                c.type = 'text/javascript';
                c.charset = 'utf-8';
                c.src = 'https://rec.smartlook.com/recorder.js';
                h.appendChild(c);
            })(document);
            window.smartlook('init', 'f533ea7bf4b36e705beb1e784b939375e4655cd6');
        };
        return SmartLookAnalyser;
    }());
    function Load() {
        window["SmartLookAnalyser"] = new SmartLookAnalyser();
    }
    SmartLook.Load = Load;
})(SmartLook || (SmartLook = {}));
SmartLook.Load();
//# sourceMappingURL=smartlook.js.map