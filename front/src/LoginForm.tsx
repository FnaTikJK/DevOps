import LockOutlined from "@ant-design/icons/lib/icons/LockOutlined";
import UserOutlined from "@ant-design/icons/lib/icons/UserOutlined";
import {Button, Form, Input, Radio } from "antd";
import {ErrorMes, User} from "./App";
import {debug} from "util";
import {useState} from "react";
import {api, AuthInfo} from "./api";
import {AxiosResponse} from "axios";

type Props = {
    onSetUser: (user: User) => void,
    onPushError: (error: ErrorMes) => void,
}

export const LoginForm = (props: Props) => {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [isReg, setIsReg] = useState<boolean>(false);

    const reqRules = [{ required: true, message: 'Неправильный формат', pattern: new RegExp("^[0-9a-zA-Z!]*$")}];

    const onAuth = async (values: {Login: string, Password: string}) => {
        setIsLoading(true);

        if (values.Login.indexOf(" ") > -1 || values.Password.indexOf(" ") > -1
            || values.Login.length === 0 || values.Password.length === 0) {
            props.onPushError({title: "Неправильные значения", message: "Поля не должны содержать пробелы"});
            setIsLoading(false);
        }

        try {
            let response = isReg
                ? await api.acc.reg(values.Login, values.Password)
                : await api.acc.auth(values.Login, values.Password);

            props.onSetUser({id: response.data.id, token: response.data.jwtToken});
        } catch (e) {
            props.onPushError({
                // @ts-ignore
                title: `${e.response.status} ${e.response.statusText}`,
                // @ts-ignore
                message: e.response.data}
            );
            setIsLoading(false)
        }


    }

    return (
        <>
            <Form name="normal_login" initialValues={{ remember: true }} onFinish={onAuth} disabled={isLoading}>
                <Form.Item>
                    <Radio.Group defaultValue="login" onChange={(e) => setIsReg(e.target.value === "reg")}>
                        <Radio.Button value={"login"}>Вход</Radio.Button>
                        <Radio.Button value={"reg"}>Регистрация</Radio.Button>
                    </Radio.Group>
                </Form.Item>
                <Form.Item name="Login" rules={reqRules}>
                    <Input
                        prefix={<UserOutlined />}
                        placeholder="Логин"
                    />
                </Form.Item>
                <Form.Item name="Password" rules={reqRules}>
                    <Input
                        prefix={<LockOutlined />}
                        type="password"
                        placeholder="Пароль"
                    />
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit" className="login-form-button" loading={isLoading}>
                        {isReg ? "Зарегистрироваться" : "Войти"}
                    </Button>
                </Form.Item>
            </Form>
        </>
    );
}