// Фреймворки
import React from "react";
import { observer } from "mobx-react-lite";
import Dropdown, { IItem } from "../Dropdown/Dropdown";
import { Api } from "../../Core/api";

import styles from "./Header.module.css";
import workspaceStore from "../../Core/WorkspaceStore";
import loginStore from "../../Core/LoginStore";

// Компонент Шапка
interface IHeaderProps {
  user: string;
}

const Header: React.FC<IHeaderProps> = observer(({ user }) => {
  const [items, setItems] = React.useState<IItem[]>([]);

  React.useEffect(() => {
    let starterSection: IItem[] = [{DisplayName:"Система", Value:"-1"}];
    Api.postAuth("Assembly/list",{}).then(async (data) => {
        var result = await data.json();
        setItems(starterSection.concat(result.dtos.filter((r:any) => r.isActive).map((r:any) => {return {DisplayName: r.name, Value: r.id }})) );
    });
  }, []);

  const handleSelect = (item: IItem) => {
    console.log(item);
    workspaceStore.load(Number(item.Value));
  };

  return (
    <div className={styles.header}>
      <Dropdown items={items} onSelect={handleSelect} />
      <div className={styles.user}><span style={{cursor:"pointer"}} onClick={()=> loginStore.logOut()}>Выйти</span></div>
    </div>
  );
});

export default Header;