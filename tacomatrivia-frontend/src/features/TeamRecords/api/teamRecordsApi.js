import {http} from '@shared/api/http';

export const teamRecordsApi ={
    list: async (params) => {
        const data = await http.get('/teamrecords', params);
        if(Array.isArray(data)) return {items: data, total: undefined};
        return {items: data.items ?? [], total: data.total}
    },
    create: async (body) => {
        const {headers, data} = await http.post('/teamrecords', body);
        const loc = headers?.location;
        const id  = loc?.split('/').pop();
    },
    edit: async(id,body) => {
        const{status} = await http.put(`/teamrecords/${id}`,body);
        return status;
    },
    delete: async(id) => {
        const {status} = await http.del(`/teamrecords/${id}`);
        return status;
    }

}