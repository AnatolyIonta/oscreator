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

interface ILocalizableStrings{
    Nav: ILocalizableNav;
    AsemblyPage: ILocalizableAsemblyPage;
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