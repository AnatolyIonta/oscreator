import React from "react";
import { observer } from "mobx-react-lite";
import styles from "./Dropdown.module.css";

// Компонент Дропдаун
interface IDropdownProps {
    items: IItem[];
    onSelect: (item: IItem) => void;
}
  
export interface IItem
{
    DisplayName: string, 
    Value: string
}

const Dropdown: React.FC<IDropdownProps> = observer(({ items, onSelect }) => {
    const handleSelect = (event: React.ChangeEvent<HTMLSelectElement>) => {
      console.log("+++1");
      const selectedItem = items.find((item) => item.Value == event.target.value);

      if (selectedItem) {
        onSelect(selectedItem);
      }
    };
  
    return (
      <select className={styles.dropdown} onChange={handleSelect}>
        {items.map((item) => (
          <option key={item.Value} value={item.Value}>
            {item.DisplayName}
          </option>
        ))}
      </select>
    );
});

export default Dropdown;