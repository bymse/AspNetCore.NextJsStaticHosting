import {useRouter} from "next/router";

export default function Id() {
    const router = useRouter()
    return <>
        not id: {router.query.id}
    </>
}