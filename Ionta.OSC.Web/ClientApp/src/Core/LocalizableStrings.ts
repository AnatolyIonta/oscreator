export const LanguageRu : ILocalizableStrings = {
    Nav: {
        Setting: "Настройки",
        Libary: "Модули",
        Marketplace: "Маркетплейс",
        About: "О создателях",
        Logs:"Логи",
    },
    AsemblyPage:{
        description: "Выберете файл в формате .dll" ,
        loadButton: "Загрузить модуль",
        showInformationModule: "Показать информацию по модулям",
        disableModule:"Деактивировать",
        enambleModule: "Активировать",
        deleteModule: "Удалить",
        moduleLoader: "Загрузчик модулей",
        deleteWarnning: "Вы уверены, что хотите удалить модуль?",
        enter: "Открыть"
    },
    SettingPage:{
        users:"Пользователи",
        title:"Настройки",
        description:"Сменить пароль",
        errorExternal:"Ошибка при смене пароля",
        errorInternal:"Пароли не совпадают",
        success:"Пароль успешно изменён",
        newPassword:"Новый пароль",
        oldPassword:"Старый пароль",
        passwordRepeat:"Повторите пароль",
        changePassword:"Сменить пароль",
        logOut:"Выйти из аккаунта"
    },
    LoginPage:{
        login: "Логин:",
        password: "Пароль:",
        entrance: "Вход"
    }, 
    ModulePage:{
        refresh: "Обновить",
        moduleTitle: "Модуль",
        changeNameModule: "Изменить название модуля:",
        save: "Сохранить",
        savedSucces: "Изменения успешно сохранены!",
        savedError: "Произошла ошибка при измении данных",
        isActive: "(Активен)",
        isNotActive: "(Не активен)",
        modulNameCantBeEmpty: "Название модуля не может быть пустым!"
    }
}

interface ILocalizableNav{
    Setting: string;
    Libary: string;
    Marketplace: string;
    About: string;
    Logs: string;
}

interface ILocalizableAsemblyPage{
    description: string;
    loadButton: string;
    showInformationModule: string;
    disableModule:string;
    enambleModule:string;
    deleteModule:string;
    moduleLoader: string;
    deleteWarnning: string;
    enter: string;
}

interface ILocalizableSettingPage{
    users:string;
    title:string;
    description: string;
    errorInternal: string;
    errorExternal: string;
    success: string;
    newPassword: string;
    oldPassword: string;
    passwordRepeat: string;
    changePassword: string;
    logOut: string;
}

interface ILocalizableLoginPage{
    login: string;
    password: string;
    entrance: string;
}

interface IModulePage{
    refresh: string;
    moduleTitle: string;
    changeNameModule: string;
    save: string;
    savedSucces: string;
    savedError: string;
    isActive: string;
    isNotActive: string;
    modulNameCantBeEmpty: string;
}

interface ILocalizableStrings{
    Nav: ILocalizableNav;
    AsemblyPage: ILocalizableAsemblyPage;
    SettingPage: ILocalizableSettingPage;
    LoginPage: ILocalizableLoginPage;
    ModulePage: IModulePage;
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