var Site;
(function (Site) {
    var Test = /** @class */ (function () {
        function Test() {
        }
        Test.prototype.Add = function (x, y) {
            var z = x + y;
            console.log(z);
        };
        return Test;
    }());
    function Load() {
        window["Test"] = new Test();
    }
    Site.Load = Load;
})(Site || (Site = {}));
Site.Load();
//# sourceMappingURL=site.js.map