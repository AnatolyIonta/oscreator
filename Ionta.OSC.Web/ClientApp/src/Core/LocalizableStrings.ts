export const LanguageRu : ILocalizableStrings = {
    Nav: {
        Setting: "Настройки",
        Libary: "Модули",
        Marketplace: "Маркетплейс",
        About: "О создателях"
    }
}



interface ILocalizableNav{
    Setting: string;
    Libary: string;
    Marketplace: string;
    About: string;
}

interface ILocalizableStrings{
    Nav: ILocalizableNav;
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