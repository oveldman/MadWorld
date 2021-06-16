namespace SmartLook {
    class SmartLookAnalyser {
        public Init() {
            (window as any).smartlook || (function (d) {
                var o: any = (window as any).smartlook = function () { o.api.push(arguments) }, h = d.getElementsByTagName('head')[0];
                var c: any = d.createElement('script'); o.api = new Array(); c.async = true; c.type = 'text/javascript';
                c.charset = 'utf-8'; c.src = 'https://rec.smartlook.com/recorder.js'; h.appendChild(c);
            })(document);
            (window as any).smartlook('init', 'f533ea7bf4b36e705beb1e784b939375e4655cd6');
        }
    }

    export function Load() {
        window["SmartLookAnalyser"] = new SmartLookAnalyser();
    }
}

SmartLook.Load();