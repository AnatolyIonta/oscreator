import ISetting from "../../../Core/ISetting";
import Strings from "../../../Core/LocalizableStrings";
import {ReactComponent as Users} from "../../../Icon/groups.svg";

const settings : ISetting[] = [
    {
        icon: <Users />,
        caption: Strings.SettingPage.users,
        url:"users"
    }
]

export default settings;