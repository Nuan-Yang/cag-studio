if (self == top) {
    window.cag = chrome.webview.hostObjects.sync;
    window.form = cag.Webview2Common;
}

window.addEventListener('DOMContentLoaded', () => {
    var form = {};
    form.drag = false;
    document.body.addEventListener('mousedown', evt => {
        const { target } = evt;
        const appRegion = getComputedStyle(target)['-webkit-app-region'];
        if (appRegion === 'drag' && !form.drag) {
            form.drag = true;
        }
    });
    //document.body.addEventListener('mouseup', evt => {
    //    const { target } = evt;
    //    const appRegion = getComputedStyle(target)['-webkit-app-region'];
    //    if (appRegion === 'drag' && form.drag) {
    //        form.drag = false;
    //        cag.Webview2Common.IsDrag = false;
    //    }
    //});
    //document.body.addEventListener('mouseleave', evt => {
    //    const { target } = evt;
    //    const appRegion = getComputedStyle(target)['-webkit-app-region'];
    //    if (appRegion === 'drag' && form.drag) {
    //        form.Drag = false;
    //        cag.Webview2Common.IsDrag = false;
    //    }
    //});
    document.body.addEventListener('mousemove', evt => {
        const { target } = evt;
        const appRegion = getComputedStyle(target)['-webkit-app-region'];
        if (appRegion === 'drag' && form.drag) {
            form.drag = false;
            cag.Webview2Common.MoveForm();
            evt.preventDefault();
        }
    });

    document.body.addEventListener('dblclick', evt => {
        const { target } = evt;
        const appRegion = getComputedStyle(target)['-webkit-app-region'];
        if (appRegion === 'drag') {
            postMessage({ action: "MaximizeOrNomalForm" });
            cag.Webview2Common.MaximizeOrNomalForm();
        }
    });

});