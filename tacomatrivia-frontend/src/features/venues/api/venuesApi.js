import { http } from '@shared/api/http';

// Server accepts: q, page, pageSize
// Response may be either an array OR { items, total } depending on your implementation.
export const venuesApi = {
  list: async (params) => {
    const data = await http.get('/venues', params);
    if (Array.isArray(data)) return { items: data, total: undefined };
    return { items: data.items ?? [], total: data.total };
  },
};
