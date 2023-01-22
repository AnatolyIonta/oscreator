import { Api } from "./api";

export let ApiDomen = "";

export async function setAppDomen() {
    ApiDomen = await getHost();
}

async function getHost() : Promise<string>{
    let status = false;
    if((await fetch("/user/status",{method: 'POST'})).status === 200) status = true;
    let protocol = window.location.protocol;
    let host = window.location.hostname;
    let port = status ? window.location.port : 5001;
    return `${protocol}//${host}:${port}`;
}