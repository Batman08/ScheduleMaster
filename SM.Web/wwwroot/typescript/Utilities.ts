class Utilities {
    public static Sm_XMLHttpRequest(xhr:XMLHttpRequest, body: any): void {
        xhr.onloadend = () => alert("Hide Load Icon");
        xhr.onerror = () => alert("Hide Load Icon");
        xhr.onabort= () => alert("Hide Load Icon");
        xhr.ontimeout = () => alert("Hide Load Icon");
        xhr.send(JSON.stringify(body));
    }

    public static GetTextColourContrast(hexcolor) {
        var r = parseInt(hexcolor.substring(1, 3), 16);
        var g = parseInt(hexcolor.substring(3, 5), 16);
        var b = parseInt(hexcolor.substring(5, 7), 16);
        var yiq = ((r * 299) + (g * 587) + (b * 114)) / 1000;
        return (yiq >= 128) ? 'black' : 'white';
    }

    public static SetCookie(cname, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);

        var cvalue = value + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
        document.cookie = cname + "=" + cvalue;
    }

    public static GetCookie(cname) {
        var value = "";
        if (document.cookie) {
            var index = document.cookie.indexOf(cname);
            if (index != -1) {
                var namestart = (document.cookie.indexOf("=", index) + 1);
                var nameend = document.cookie.indexOf(";", index);
                if (nameend == -1) { nameend = document.cookie.length; }
                value = document.cookie.substring(namestart, nameend);
            }
        }
        return value;
    }

    public static DeleteCookie = function (cname) {
        Utilities.SetCookie(cname, '', -1);
    }
}