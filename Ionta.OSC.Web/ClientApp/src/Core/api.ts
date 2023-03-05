import { ApiDomen } from "./Configure";
import loginStore from "./LoginStore";

export class Api{
  static async postNoBodyAuth(url:string) : Promise<Response>{
      return await fetch(ApiDomen+"/"+url,{
          method: 'POST',
          headers: {
            Authorization: "Bearer "+loginStore.token,
            'Content-Type': 'application/json;charset=utf-8'
          },
          body:"{}"
        });
  }

  static async postAuth(url:string, body:any) : Promise<Response>{
    console.log(url);
    
    return await fetch(ApiDomen+"/"+url,{
        method: 'POST',
        headers: {
          Authorization: "Bearer " + loginStore.token,
          'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(body)
      });
  }
}