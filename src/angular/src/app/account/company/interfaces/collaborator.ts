export class CollaboratorResponse {
  collaboratorId: string = "";
  companyId: string = "";
  userId: string = "";
  userName: string = "";
  email: string = "";
  userRole: string = "";
}

export class InviteCollaborator {
  companyId: string = "";
  email: string = "";
  userRole: string = "";
}

export class Collaborator {
  id: any = undefined;
  companyId: string = "";
  userId: string = "";
  userRole: string = "";
}
