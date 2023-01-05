export const ApiDomen = getHost();

function getHost(){
    let protocol = window.location.protocol;
    let host = window.location.hostname;
    let port = 5001;
    return `${protocol}//${host}:${port}`;
}