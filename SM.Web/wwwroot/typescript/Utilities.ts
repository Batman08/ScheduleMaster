//import * as $ from "bootstrap";
class Utilities {
    public static Sm_XMLHttpRequest(xhr: XMLHttpRequest, body: any): void {
        xhr.onloadstart = (ev: ProgressEvent) => this.AjaxLoadingIconShow();
        xhr.onloadend = (ev: ProgressEvent) => this.AjaxLoadingIconHide();
        xhr.onerror = (ev: ProgressEvent) => this.AjaxLoadingIconHide();
        xhr.onabort = (ev: ProgressEvent) => this.AjaxLoadingIconHide();
        xhr.ontimeout = (ev: ProgressEvent) => this.AjaxLoadingIconHide();
        xhr.send(JSON.stringify(body));
    }

    private static AjaxLoadingIconShow = () => $('#sm-loading').fadeIn('slow');
    private static AjaxLoadingIconHide = () => $('#sm-loading').fadeOut('slow');

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

    public static RemoveBtnPointerEvents(btnEl: HTMLButtonElement): void {
        btnEl.style.pointerEvents = "none";
        btnEl.classList.add("opacity-50");
    }

    public static EnableBtn(btnEl: HTMLButtonElement): void {
        btnEl.style.pointerEvents = "auto";
        btnEl.disabled = false;
    }

    public static DisableBtn(btnEl: HTMLButtonElement): void {
        btnEl.style.pointerEvents = "none";
        btnEl.disabled = true;
    }

    public static ShowPanel(loadingPanelId: string, mainPanelId: string): void {
        const loadingPanel = document.querySelector('#' + loadingPanelId) as HTMLDivElement;
        const mainPanel = document.querySelector('#' + mainPanelId) as HTMLDivElement;

        loadingPanel.style.display = 'none';
        mainPanel.style.display = 'block';
    }

    /*  Bootstrap Modals  */

    public static BTSP_GetModal(selector: string): bootstrap.Modal {
        const modalElement = document.querySelector(selector) as HTMLDivElement
        return new bootstrap.Modal(modalElement);
    }

    public static BTSP_OpenModal(modal: bootstrap.Modal): void {
        modal.show();
    }

    public static BTSP_CloseModal(modal: bootstrap.Modal): void {
        modal.hide();
    }
}