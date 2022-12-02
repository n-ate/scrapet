(function () {
    let _eval = window.eval;
    window.eval = function (jsString) {
        if (jsString.indexOf("\n//# logErrors") >= 0) {
            try {
                return _eval(jsString);
            }
            catch (ex) {
                console.error(ex);
            }
        }
        else {
            return _eval(jsString);
        }
    };
})();