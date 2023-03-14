import PageStore from "./PageStore"; 

const url = window.location.href.split('/');
const id = url[url.length - 1];
var pageStore = new PageStore(id);

export default pageStore;