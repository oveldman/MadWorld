namespace Site {
    class Test {
        public Add(x: number, y: number) {
            var z: number = x + y;
            console.log(z);
        }
    }

    export function Load() {
        window["Test"] = new Test();
    }
}

Site.Load();