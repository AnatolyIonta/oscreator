import styles from "./Button.module.css";

export enum ButtonStyles{
    classic,
    red
}

export default function Button(props:{title:string, onClick:(e:any)=>void, buttonStyle?:ButtonStyles}) {
    let classStyle = selectStyle(props.buttonStyle)
    return(
        <div onClick={props.onClick} className={classStyle}>{props.title}</div>
    );
};

export function ButtonFileLoad(props:{title:string, onChange:(e:any) => void}){
    return(
        <label className={styles.classic}>
            <input className='displayNone' type="file" onChange={props.onChange}/>
            <div>{props.title}</div>
        </label>
    );
};

function selectStyle(inputStyles: ButtonStyles | undefined) {
    if(inputStyles === undefined || inputStyles! == ButtonStyles.classic)
        return styles.classic;
    else if(inputStyles == ButtonStyles.red)
        return styles.red;
}