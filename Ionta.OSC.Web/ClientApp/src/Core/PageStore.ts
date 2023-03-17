import { action, makeObservable, observable } from "mobx";
import { Api } from "./api"; 

class PageStore<T extends IEntityStore> {
    url:string;
    entity: T | null = null;

    constructor(url:string) {
        this.url = url
        makeObservable(this)
    }

    setEntity(entity:T){
        this.entity = entity;
        this.loadModulPageInfo();
    }

    @action
    async loadModulPageInfo() {
        const moduleId = {id: this.entity?.id};
    
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