import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class RoleService {

    constructor(private _httpClient: HttpClient) { }
    getRoles() {
        return this._httpClient.get('https://localhost:44359/api/Role/All', this.getHeaders());
    }
    updateRole(payload: any) {
        return this._httpClient.put('https://localhost:44359/api/Role/Update', payload, this.getHeaders());
    }
    createRole(payload: any) {
        return this._httpClient.post('https://localhost:44359/api/Role/Create', payload, this.getHeaders());
    }
    deleteRole(id: number) {
        return this._httpClient.delete('https://localhost:44359/api/Role/Delete/' + id, this.getHeaders());
    }
    getRolePrivileges(roleId: number) {
        return this._httpClient.get("" + `${roleId}`, this.getHeaders())
    }   
    saveRolePrivileges(rolePrivilege: any) {
        return this._httpClient.post("", rolePrivilege, this.getHeaders());
    }
    removeRolePrivileges(id: number) {
        return this._httpClient.delete('' + id, this.getHeaders());
    }
    private loginHeaders(): any {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json;',
                'Accept': 'application/json;',
            })
        };
    }
    private getHeaders(token?: string): any {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json;',
                'Accept': 'application/json;',
                'Authorization': 'bearer ' + token
            })
        };
    }
}