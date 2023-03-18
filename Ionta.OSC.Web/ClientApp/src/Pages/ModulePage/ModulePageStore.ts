import { makeAutoObservable } from "mobx";
import PageStore, { IEntityStore } from "../../Core/PageStore";

class ModuleEntity implements IEntityStore{
    id: number = 0;
    name: string = "";
    isActive: boolean = false;

    constructor(){
        makeAutoObservable(this);
    }
}

const store = new PageStore<ModuleEntity>("assembly/infoCurrentAssembly");

export default store;