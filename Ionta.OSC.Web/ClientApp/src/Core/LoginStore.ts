import { observable, reaction } from "mobx";

// это наш стор для логина
const loginStore = observable({
    token : window.localStorage.getItem('jwt'),
    
    get loggedIn():boolean {
        return (this.token !== undefined && this.token !== null);
    },

    logIn(token: string) {
        this.token = token;
        console.log(this.token);
        console.log(this.loggedIn);
    },
    logOut() {
        this.token = null;
    }


});

reaction(
    () => loginStore.token,
    token => {
        if (token) {
            window.localStorage.setItem('jwt', token);
        } else {
            window.localStorage.removeItem('jwt');
        }
    }
);

export default loginStore;