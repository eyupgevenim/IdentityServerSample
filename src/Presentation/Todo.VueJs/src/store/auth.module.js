import { UserManager, WebStorageStateStore, User } from 'oidc-client';

const AUTHORITY_DOMAIN = 'http://localhost:5101';
const BASE_DOMAIN = 'http://localhost:5103';

const config = {
    userStore: new WebStorageStateStore({ store: window.localStorage }),
    authority: AUTHORITY_DOMAIN,
    client_id: 'js',
    redirect_uri: `${BASE_DOMAIN}/callback.html`,
    automaticSilentRenew: true,
    silent_redirect_uri: `${BASE_DOMAIN}/silent-renew.html`,
    response_type: 'code',
    scope: 'openid profile todo',
    post_logout_redirect_uri: `${BASE_DOMAIN}/`,
    filterProtocolClaims: true,
};

const userManager = new UserManager(config);
const initialState = { status: { isAuthenticated: false }, user: null, userManager: userManager };
const signinRedirect = ({ commit }, returnPath) => returnPath ? userManager.signinRedirect({ state: returnPath }) : userManager.signinRedirect();
const getUser = async () => await userManager.getUser();

export const auth = {
    namespaced: true,
    state: initialState,
    actions: {
        getUser: getUser,
        login: signinRedirect,
        logout: ({ commit }) => {
            commit('logout');
            userManager.signoutRedirect();
        },
        authenticate: async ({ commit }, returnPath) => {
            let user = await getUser(); //see if the user details are in local storage
            user
                ? commit('loginSuccess', user)
                : signinRedirect(returnPath);
        },
    },
    mutations: {
        loginSuccess(state, user) {
            state.status.isAuthenticated = true;
            state.user = user;
        },
        logout(state) {
            state.status.isAuthenticated = false;
            state.user = null;
        }
    }
};
