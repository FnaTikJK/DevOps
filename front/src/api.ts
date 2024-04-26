import axios from "axios";

const Pref = "api"

export const api = {
    acc: {
        my: async () => axios.get<AuthInfo>(`${Pref}/Accounts`),
        reg: async (login: string, password: string) => axios.post<AuthInfo>(`${Pref}/Accounts/Reg`, {
            login,
            password
        }),
        auth: async (login: string, password: string) => axios.post<AuthInfo>(`${Pref}/Accounts/Auth`, {
            login,
            password
        })
    }
}

export type AuthInfo = {
    id: string,
    jwtToken: string,
};