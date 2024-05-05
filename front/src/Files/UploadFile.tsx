import {api} from "../api";
import {useEffect, useRef, useState} from "react";


type Props = {
    onSetLoading: (flag: boolean) => void,
    onSetFileIds: (fileIds: string[]) => void,
    isEditingMessage: boolean,
    initEditMessageFileIds?: string[],
    clearTrigger: boolean,
}

export const UploadFile = (props: Props) => {
    const [editState, setEditState] = useState<"default" | "notEdit" | "edit">("default");
    const [initEditMessageFileIds, setInitEditMessageFileIds] = useState<string[]>([]);
    const inputRef = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        setEditState(props.isEditingMessage ? "notEdit" : "default");
        setInitEditMessageFileIds(props.initEditMessageFileIds ?? []);
    }, [props.isEditingMessage]);

    useEffect(() => {
        if (inputRef.current)
            inputRef.current.value = ""
    }, [props.clearTrigger]);

    const onUpload = async (files: FileList | null) => {
        if (!files)
            return;

        props.onSetLoading(true);
        try {
            const formData = new FormData();
            for (let i = 0; i < files.length; i++) {
                formData.append("Files", files[i]); // Название (Files) должно совпадать с полем, в модельке Запроса
            }
            const response = await api.statics.upload(formData);
            props.onSetFileIds(response.data.fileIds);
            props.onSetLoading(false);
        } catch (e) {
            console.log(e);
        }
    }

    const onCancelEditing = () => {
        setEditState("notEdit");
        props.onSetFileIds(initEditMessageFileIds!);
    }

    if(editState === "notEdit")
        return (
            <>
                <button onClick={() => setEditState("edit")}>Изменить файлы</button>
                Файлов - {props.initEditMessageFileIds?.length}
            </>
        )

    return (
        <>
            <input
                type="file"
                multiple
                onChange={e => onUpload(e.target.files)}
                ref={(input) => inputRef.current = input}
            />
            {editState === "edit" && <button onClick={onCancelEditing}>Отменить</button>}
        </>
    )
}