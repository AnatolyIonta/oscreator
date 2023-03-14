import { action, makeObservable, observable } from "mobx";
import { Api } from "../../Core/api"; 

class PageStore {
    @observable name: string;
    @observable id: string;
    @observable isActive: boolean;

    constructor(id: string) {
        this.name = '';
        this.id = id;
        this.isActive = false;
        makeObservable(this)
    }

    @action
    async loadModulPageInfo() {
        const moduleId = {id: this.id};
    
        let result = await Api.postAuth("assembly/infoCurrentAssembly", moduleId);
        if(result.status === 200) {
            let json = await result.json();
            this.name = json.name;
            this.id = json.id;
            this.isActive = json.isActive;
        }
    }
}

export default PageStore;