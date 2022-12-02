function Events(ns, self) {

    self.clicks = [];
    document.addEventListener("click", function (ev) {
        let target = ev.originalTarget;
        ns.selection.
            clicks.push({
            });
    });

}