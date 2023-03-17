import "../../App.css";

interface IHeaderProps{
    title: string
    children?: any
}

function HeaderPage(props:IHeaderProps){
    return(
        <div className='row justBetween alignCenter' style={{marginBottom:"30px"}}>
            <h3>{props.title}</h3>
            <div className="row gap">
                {props.children}
            </div>
        </div>
    )
}

export default HeaderPage;