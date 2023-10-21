import {useRouter} from "next/router";

export default function Id() {
    const router = useRouter()
    return <>
        id: {router.query.id}
    </>
}