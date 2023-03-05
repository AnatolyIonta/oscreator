import { action, makeObservable, observable } from "mobx";
import { Api } from "./api";

class ListStore{
    @observable data:any[] | null;
    @observable count:number;
    schema: string;

    constructor(schema:string){
        this.schema = schema;
        this.data = null;
        this.count = 0;
        makeObservable(this)
    }

    @action
    async load(){
        let result = await Api.postAuth(this.schema+"/list",{});
        if (result.status == 200) {
            let json = await result.json();
            console.log('json',json);
            
            this.data = json.dtos;
            this.count = json.count;
        }
        console.log('data',this.data);
    }
}

export default ListStore;