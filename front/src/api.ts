import axios from "axios";
import {Upload} from "antd";

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
        }),
        logout: async () => axios.post(`${Pref}/Accounts/Logout`),
    },
    messages: {
        getById: async(id: string) => axios.get<Message>(`${Pref}/Messages/${id}`),
        getAll: async () => axios.get<Message[]>(`${Pref}/Messages`),
        post: async (req: MessageCreateReq) => axios.post(`${Pref}/Messages`, req),
        delete: async (id: string) => axios.delete(`${Pref}/Messages/${id}`),
    },
    statics: {
        upload: async (formData: FormData) => axios.postForm<UploadResult>(`${Pref}/Statics/Upload`, formData),
        download: async (id: string) => axios.post(`${Pref}/Statics/Download/${id}`, {}, {responseType: "blob"})
    }
}

export type AuthInfo = {
    id: string,
    jwtToken: string,
    login: string,
};

export type Message = {
    id: string,
    author: {
        id: string,
        login: string,
    },
    files: File[],
    text: string,
    dateTime: Date,
    isEdited: boolean,
}

export type File = {
    id: string,
    key: string,
    fileName: string,
    contentType: string,
}

export type MessageCreateReq = {
    id?: string,
    text?: string,
    fileIds?: string[],
}

export type UploadResult = {
    fileIds: string[]
}