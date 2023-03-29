import Strings from "./LocalizableStrings";
import INavItem from "./NavItem";

import {ReactComponent as Library} from "../Icon/library.svg";
import {ReactComponent as Setting} from "../Icon/settings.svg";
import {ReactComponent as Info} from "../Icon/info.svg";
import {ReactComponent as Logs} from "../Icon/problem.svg";
import { action, makeObservable, observable } from "mobx";
import { Api } from "./api";

const systemPages : INavItem[] = [
    {
        name: Strings.Nav.Libary,
        url: "/additions",
        icon: <Library fill='#e4e9ed' stroke='#e4e9ed'/>
    },
    {
        name: Strings.Nav.Setting,
        url: "/settings",
        icon: <Setting fill='#e4e9ed' stroke='#e4e9ed'/>
    },
    {
        name: Strings.Nav.About,
        url: "/about",
        icon: <Info fill='#e4e9ed' stroke='#e4e9ed'/>
    },
    {
        name: Strings.Nav.Logs,
        url: "/logs",
        icon: <Logs fill='#e4e9ed' stroke='#e4e9ed'/>
    },
]

class WorkspaceStore {
    pages: INavItem[] = systemPages;
  
    constructor() {
      makeObservable(this, {
        pages: observable,
        load: action,
      });
    }
  
    async load(id: number) {
      if (id === -1) {
        this.pages = systemPages;
        return;
      }
  
      try {
        const url = "workspace/getpages";
        const body = { id:id };
        const response = await Api.postAuth(url, body);
  
        // Обновляем список страниц при успешной загрузке данных
        let result = await response.json()
        this.pages = result.dtos.map((d:any) => {return {name: d.name, url: d.url, icon:<></> }});
        console.log(this.pages);
        
      } catch (error) {
        console.error("Failed to load pages:", error);
      }
    }
} 

const workspaceStore = new WorkspaceStore();

export default workspaceStore;