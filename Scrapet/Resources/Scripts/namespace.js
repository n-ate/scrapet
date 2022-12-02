window.__SCRAPET = (function (debug, registry) {
    let private = {
        _maxRegistrationMS: 2000,
        _expected: [...registry],
        _pending: [...registry],
        _registering: [],
        _namespace: { isDebug: debug },
        _destruct: function () {
            this._hide();
            public.Register = null; //remove public Register method
            private = null; //remove private members
        },
        _hide: function () {
            delete window.__SCRAPET; //remove __SCRAPET from window
        },
        _register: function (name) {
            let permittedIndex = this._expected.indexOf(name);
            let pendingIndex = this._pending.indexOf(name);
            if (pendingIndex >= 0) { //first time registering permitted object
                this._pending.splice(pendingIndex, 1); //remove index
                this._namespace[name] = {};
                if (this._pending.length == 0) this._hide();
                return this._namespace;
            }
            else if (permittedIndex >= 0) { //permitted object already registered; destroy everything
                if (debug) throw new Error(`Object already registered. Name: ${name}`);
                else this._destruct();
            }
            else { //name is not permitted
                if (debug) throw new Error(`Object is not permitted. Name: ${name}`);
            }
        }
    };

    let public = { Register: private._register.bind(private) };

    setTimeout(function () {
        let error = null;
        if (private != null) {
            if (debug && private._pending.length > 0) {
                error = `The following expected objects were not registered: ${private._pending.join(", ")}.`;
            }
            private._destruct();
        }
        if (error) throw new Error(error);
    }, private._maxRegistrationMS);

    return public;
})(true, ["events", "selections"]);