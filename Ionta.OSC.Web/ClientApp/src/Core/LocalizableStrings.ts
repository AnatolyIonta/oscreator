export const LanguageRu : ILocalizableStrings = {
    Nav: {
        Setting: "Настройки",
        Libary: "Модули",
        Marketplace: "Маркетплейс",
        About: "О создателях"
    },
    AsemblyPage:{
        description: "Выберете файл в формате .dll" ,
        loadButton: "Загрузить модуль",
        disableModule:"Деактивировать",
        enambleModule: "Активировать",
        deleteModule: "Удалить",
    },
    SettingPage:{
        description:"Сменить пароль",
        errorExternal:"Ошибка при смене пароля",
        errorInternal:"Пароли не совпадают",
        seccses:"Пароль успешно изменён",
        newPassword:"Новый пароль",
        oldPassword:"Старый пароль",
        passwordRepeat:"Повторите пароль",
        changePassword:"Сменить пароль",
        logOut:"Выйти из аккаунта"
    }
}



interface ILocalizableNav{
    Setting: string;
    Libary: string;
    Marketplace: string;
    About: string;
}

interface ILocalizableAsemblyPage{
    description:string;
    loadButton:string;
    disableModule:string;
    enambleModule:string;
    deleteModule:string;
}

interface ILocalizableSettingPage{
    description:string;
    errorInternal:string;
    errorExternal:string;
    seccses:string;
    newPassword:string;
    oldPassword:string;
    passwordRepeat:string;
    changePassword:string;
    logOut:string;
}

interface ILocalizableStrings{
    Nav: ILocalizableNav;
    AsemblyPage: ILocalizableAsemblyPage;
    SettingPage: ILocalizableSettingPage;
}

export class LocalizableStringsManager{
    private currentLanguage : ILocalizableStrings;

    constructor(language: ILocalizableStrings = LanguageRu){
        this.currentLanguage = language;
    }

    setStrategy(language : ILocalizableStrings){
        this.currentLanguage = language;
    }

    get strings(){
        return this.currentLanguage;
    }
}

export const LStrings = new LocalizableStringsManager();

const Strings = LStrings.strings;

export default Strings;