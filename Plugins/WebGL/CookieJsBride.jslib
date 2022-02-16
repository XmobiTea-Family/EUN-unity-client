mergeInto(LibraryManager.library, { 
    SetCookie: function(cname, cvalue, expiredMs) {
        cname = Pointer_stringify(cname);
        cvalue = Pointer_stringify(cvalue);

        var d = new Date();
        d.setTime(d.getTime() + expiredMs);
        var expires = "expires=" + d.toGMTString();
        window.document.cookie = cname + "=" + cvalue + "; " + expires + "/";
    },

    GetCookie: function(cname) {
        cname = Pointer_stringify(cname);
        var name = cname + "=";
        var cArr = window.document.cookie.split(';');
        for (var i = 0; i < cArr.length; i++) {
            var c = cArr[i].trim();
            if (c.indexOf(name) == 0)
                return c.substring(name.length, c.length);
        }
        return "";
    },

    DeleteCookie: function(cname) {
        cname = Pointer_stringify(cname);
        var d = new Date();
        d.setTime(d.getTime() - (1000 * 60 * 60 * 24 * 30 * 12));
        var expires = "expires=" + d.toGMTString();
        window.document.cookie = cname + "=" + "; " + expires;
    }
});