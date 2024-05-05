import {api, Message} from "../api";
import {useEffect, useState} from "react";
import {UploadFile} from "../Files/UploadFile";
import {Button, Flex, Input} from "antd";
import {CloseOutlined, SendOutlined} from "@ant-design/icons";

type TextAreaProps = {
    message?: Message,
    onCancel: () => void,
    onSend: () => void,
}

export const TextArea = (props: TextAreaProps) => {
    const [id, setId] = useState<string | undefined>(props.message?.id);
    const [text, setText] = useState<string | undefined>(props.message?.text);
    const [fileIds, setFileIds] = useState<string[] | undefined>(props.message?.files.map(f => f.id));
    const [isUploading, setIsUploading] = useState<boolean>(false);
    const [clearFilesTrigger, setClearFilesTrigger] = useState(false);

    useEffect(() => {
        setMessage(props.message);
    }, [props.message]);

    const onSend = async () => {
        const isTextEmpty = !text || text?.trim().length === 0;
        const isFilesEmpty = !fileIds || fileIds?.length === 0;
        if(isTextEmpty && isFilesEmpty)
            return;

        setIsUploading(true);
        await api.messages.post({
            id,
            text,
            fileIds,
        })
        setMessage(undefined);
        props.onSend();
        setClearFilesTrigger(!clearFilesTrigger);
        setIsUploading(false);
    }

    const setMessage = (message?: Message) => {
        setId(message?.id);
        setText(message?.text);
        setFileIds(message?.files.map(f => f.id))
    }

    return (
        <>
            <UploadFile
                onSetLoading={setIsUploading}
                onSetFileIds={setFileIds}
                isEditingMessage={!!props.message}
                initEditMessageFileIds={props.message?.files.map(f => f.id)}
                clearTrigger={clearFilesTrigger}
            />

            <Flex justify={"center"}>
                <Input.TextArea
                    autoSize={{ minRows: 1, maxRows: 8 }}
                    placeholder={"Введите сообщение"}
                    onChange={e => setText(e.target.value)}
                    value={text}
                />
                {props.message && <Button onClick={props.onCancel}><CloseOutlined /></Button>}
                <Button onClick={onSend} loading={isUploading}>
                    <SendOutlined />
                </Button>
            </Flex>
        </>
    )
}