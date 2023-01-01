import styles from "./Button.module.css";

export enum ButtonStyles{
    classic,
    red
}

export default function Button(props:{title:string, onClick:(e:any)=>void, buttonStyle?:ButtonStyles}){
    
    let classStyle;
    if(props.buttonStyle === undefined || props.buttonStyle! == ButtonStyles.classic)
        classStyle = styles.classic;
    else if(props.buttonStyle == ButtonStyles.red)
        classStyle = styles.red;
    return(
        <div onClick={props.onClick} className={classStyle}>{props.title}</div>
    )
}

export function ButtonFileLoad(props:{title:string}){
    return(
        <label className={styles.classic}>
            <input className='displayNone' type="file"/>
            <div>{props.title}</div>
        </label>
    )
}