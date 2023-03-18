import { action, makeObservable, observable } from "mobx";
import { Api } from "./api"; 

class PageStore<T extends IEntityStore> {
    url:string;
    currentId: number | null = null;
    @observable entity: T | null = null;

    constructor(url:string) {
        this.url = url
        makeObservable(this)
    }

    @action
    setEntity(id:number){
        this.currentId = id;
        this.load();
    }

    @action
    async load() {
        const moduleId = {id: this.currentId};
    
        let result = await Api.postAuth(this.url, moduleId);
        if(result.status === 200) {
            let json = await result.json();
            this.entity = json;
        }
    }
}

export interface IEntityStore{
    id:number;
}

export default PageStore;