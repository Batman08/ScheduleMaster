class Utilities {
    public static Sm_XMLHttpRequest(xhr:XMLHttpRequest, body: any): void {
        xhr.onloadend = () => alert("Hide Load Icon");
        xhr.onerror = () => alert("Hide Load Icon");
        xhr.onabort= () => alert("Hide Load Icon");
        xhr.ontimeout = () => alert("Hide Load Icon");
        xhr.send(JSON.stringify(body));
    }
}