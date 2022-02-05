import jsSHA from 'jssha';
import { CONFIG } from '@/constants/config'



export function getAuthorizationHeader() {
    const GMTString = new Date().toGMTString();

    return {
        // 'Accept-Encoding': 'gzip',
        'Content-Type': 'application/json',
        'X-Date': GMTString
    };
}
