import { http } from '@shared/api/http';

// Server accepts: q, page, pageSize
// Response may be either an array OR { items, total } depending on your implementation.
export const venuesApi = {
  list: async (params) => {
    const data = await http.get('/venues', params);
    if (Array.isArray(data)) return { items: data, total: undefined };
    return { items: data.items ?? [], total: data.total };
  },
  create: async(body) => {
    const {headers,data} = await http.post('/venues', body);
    const loc = headers?.location;
    const id = loc?.split('/').pop();
    //console.log('Created id:', id);
    
  },
  edit: async(id,body) => {
    const {status} = await http.put(`/venues/${id}`, body);
    
    console.log('Edited venues, status:', status);
    return status;
  },
  delete: async(id) => {
    const {status} = await http.del(`/venues/${id}`);
    //console.log('User was deleted, here is the status:', status)
    return status;
  }
};
