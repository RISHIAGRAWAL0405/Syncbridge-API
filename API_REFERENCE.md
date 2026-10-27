# Sync Bridge API — Complete Reference
> Base URL: `https://localhost:7000/api`
> All protected endpoints require: `Authorization: Bearer <access_token>`

---

## GLOBAL RESPONSE WRAPPER
Every endpoint returns this structure:
```json
{
  "statusCode": 200,
  "statusMessage": "200 Success",
  "errorMessage": null,
  "data": { },
  "id": null
}
```
- `statusCode` — HTTP status code
- `statusMessage` — human-readable status
- `errorMessage` — null on success, error string on failure
- `data` — response payload (null on error)
- `id` — returned ID after create/update operations

---

## ENUMS

### UserType
```
ADMIN | USER
```

### UserAuthOtpType
```
RESET_PASSWORD | EMAIL_VERIFICATION
```

---

## ═══════════════════════════════════════
## 1. AUTH  —  /api/Auth
## ═══════════════════════════════════════
> No Authorization header required

---

### POST /api/Auth/login
Login and get tokens.

**Request Body:**
```json
{
  "user_name": "string",
  "password": "string"
}
```

**Response `data`:**
```json
{
  "accessToken": "string",
  "refreshToken": "string",
  "user": {
    "userId": "guid",
    "userName": "string",
    "firstName": "string",
    "lastName": "string",
    "userType": "string",
    "email": "string",
    "mobile": "string",
    "isActive": true,
    "permissionList": [
      {
        "permission_id": "guid",
        "role_id": "guid",
        "permission_code": "string",
        "permission_name": "string",
        "module_name": "string",
        "can_view": true,
        "can_add": true,
        "can_update": true,
        "can_delete": true,
        "can_export": true
      }
    ],
    "userRole": {
      "role_id": "guid",
      "role_code": "string",
      "role_name": "string",
      "is_assignable": true
    },
    "profile_img_path": "string | null",
    "profile_banner_path": "string | null",
    "online_status": "string",
    "status_message": "string",
    "is_email_verified": true,
    "is_mobile_verified": true,
    "is_active": true
  }
}
```

---

### POST /api/Auth/refresh
Refresh access token using refresh token.

**Request Body:**
```json
{
  "refresh_token": "string"
}
```

**Response `data`:** Same as login response.

---

### POST /api/Auth/logout
Revoke refresh token.

**Request Body:**
```json
{
  "refresh_token": "string"
}
```

**Response `data`:** `null`

---

### POST /api/Auth/forgot-password
Send OTP to user's email for password reset.

**Request Body:**
```json
{
  "user_name": "string"
}
```

**Response `data`:**
```json
{
  "user_id": "guid"
}
```

---

### POST /api/Auth/reset-password
Reset password using OTP.

**Request Body:**
```json
{
  "user_id": "guid",
  "otp_code": "string",
  "new_password": "string"
}
```

**Response `data`:** `null`

---

## ═══════════════════════════════════════
## 2. USERS  —  /api/Users
## ═══════════════════════════════════════

---

### POST /api/Users
Create a new user.

**Request Body:**
```json
{
  "user_name": "string",
  "email": "string",
  "first_name": "string",
  "last_name": "string",
  "mobile": "string | null",
  "user_type": "ADMIN | USER",
  "password": "string",
  "designation_name": "string | null",
  "department_name": "string | null",
  "role_id": "guid | null"
}
```

**Response `id`:** `guid` (new user_id)

---

### PUT /api/Users/{user_id}
Update an existing user.

**URL Param:** `user_id` (guid)

**Request Body:**
```json
{
  "email": "string",
  "mobile": "string",
  "first_name": "string",
  "last_name": "string",
  "user_type": "ADMIN | USER",
  "role_id": "guid | null",
  "designation_name": "string | null",
  "department_name": "string | null"
}
```

**Response `id`:** `guid`

---

### GET /api/Users/{user_id}
Get user by ID with assigned role.

**URL Param:** `user_id` (guid)

**Response `data`:**
```json
{
  "user_id": "guid",
  "user_name": "string",
  "email": "string",
  "mobile": "string",
  "first_name": "string",
  "last_name": "string",
  "full_name": "string",
  "profile_img_url": "string",
  "profile_url": "string",
  "online_status": "string",
  "status_message": "string",
  "designation_name": "string",
  "department_name": "string",
  "is_email_verified": true,
  "is_mobile_verified": true,
  "is_active": true,
  "user_type": "string",
  "role": {
    "role_id": "guid",
    "role_code": "string",
    "role_name": "string",
    "is_assignable": true
  },
  "profile_img_path": "string | null",
  "profile_banner_path": "string | null"
}
```

---

### GET /api/Users/GetAllUsersAsync
Get all users with their assigned role.

**Response `data`:** Array of:
```json
{
  "user_id": "guid",
  "user_name": "string",
  "email": "string",
  "mobile": "string",
  "first_name": "string",
  "last_name": "string",
  "full_name": "string",
  "profile_img_url": "string",
  "profile_url": "string",
  "online_status": "string",
  "status_message": "string",
  "designation_name": "string",
  "department_name": "string",
  "is_email_verified": true,
  "is_mobile_verified": true,
  "is_active": true,
  "user_type": "string",
  "role_name": "string",
  "role_code": "string",
  "profile_img_path": "string | null",
  "profile_banner_path": "string | null"
}
```

---

### GET /api/Users/GetAllActiveUsersAsync
Get all active users.

**Response `data`:** Array of UserMasterDto (same as GET by ID response).

---

### GET /api/Users/GetAllActiveUsersByUserType?user_type={user_type}
Get active users filtered by type.

**Query Param:** `user_type` — `ADMIN` or `USER`

**Response `data`:** Array of UserMasterDto.

---

### POST /api/Users/check-username
Check if a username already exists.

**Request Body:**
```json
{
  "username": "string"
}
```

**Response `data`:**
```json
{
  "exists": true,
  "message": "Username already exists | Username is available"
}
```

---

### POST /api/Users/activeInactive
Activate or deactivate a user.

**Request Body:**
```json
{
  "user_id": "guid",
  "is_active": true
}
```

**Response `data`:** `null`

---

### POST /api/Users/send-email-verification
Send email verification OTP to user.

**Request Body:**
```json
{
  "user_id": "guid"
}
```

**Response `data`:** `null`

---

### POST /api/Users/verify-email
Verify email using OTP.

**Request Body:**
```json
{
  "user_id": "guid",
  "otp_code": "string"
}
```

**Response `data`:** `null`

---

## ═══════════════════════════════════════
## 3. MY PROFILE  —  /api/MyProfile
## ═══════════════════════════════════════
> All endpoints use JWT token to identify the logged-in user (no user_id in URL)

---

### GET /api/MyProfile/getMyProfile
Get logged-in user's profile.

**Response `data`:** UserMasterDto (same as GET /api/Users/{user_id})

---

### POST /api/MyProfile/updateProfileImage
Upload or remove profile image.
> Content-Type: `multipart/form-data`

**Form Field:** `ProfileImage` (file — jpg/jpeg/png/webp) — send empty/null to remove

**Response `data`:**
```json
{
  "profileUrl": "string | null"
}
```

---

### POST /api/MyProfile/updateProfilebanner
Upload or remove profile banner image.
> Content-Type: `multipart/form-data`

**Form Field:** `Profilebanner` (file — jpg/jpeg/png/webp) — send empty/null to remove

**Response `data`:**
```json
{
  "profileUrl": "string | null"
}
```

---

### PUT /api/MyProfile/updateProfiledetails
Update profile details.

**Request Body:**
```json
{
  "email": "string | null",
  "first_name": "string | null",
  "last_name": "string | null",
  "mobile": "string | null",
  "profile_url": "string | null"
}
```

**Response `data`:** Updated UserMasterDto

---

### POST /api/MyProfile/changePassword
Change password.

**Request Body:**
```json
{
  "old_password": "string",
  "new_password": "string"
}
```

**Response `data`:** `null`

---

### PUT /api/MyProfile/updateOnlineStatus
Update online status.

**Request Body:**
```json
{
  "online_status": "string"
}
```

**Response `data`:** `null`

---

### PUT /api/MyProfile/updateStatusMessage
Update status message.

**Request Body:**
```json
{
  "status_message": "string"
}
```

**Response `data`:** `null`

---

## ═══════════════════════════════════════
## 4. ROLES  —  /api/Roles
## ═══════════════════════════════════════

---

### POST /api/Roles
Create a new role.

**Request Body:**
```json
{
  "role_code": "string",
  "role_name": "string"
}
```

**Response `id`:** `guid` (new role_id)

---

### PUT /api/Roles/{role_id}
Update a role.

**URL Param:** `role_id` (guid)

**Request Body:**
```json
{
  "role_id": "guid",
  "role_name": "string"
}
```

**Response `id`:** `guid`

---

### GET /api/Roles/{role_id}
Get role by ID.

**URL Param:** `role_id` (guid)

**Response `data`:**
```json
{
  "role_id": "guid",
  "role_code": "string",
  "role_name": "string",
  "is_assignable": true
}
```

---

### GET /api/Roles
Get all assignable roles.

**Response `data`:** Array of Role objects.

---

### PUT /api/Roles/{role_id}/permissions
Replace all permissions for a role.

**URL Param:** `role_id` (guid)

**Request Body:**
```json
{
  "permissionsList": [
    {
      "role_id": "guid | null",
      "permission_id": "guid",
      "can_view": true,
      "can_add": true,
      "can_update": true,
      "can_delete": true,
      "can_export": true
    }
  ]
}
```

**Response `data`:** `null`

---

### GET /api/Roles/{role_id}/permissions
Get all permissions assigned to a role.

**URL Param:** `role_id` (guid)

**Response `data`:** Array of:
```json
{
  "permission_id": "guid",
  "permission_code": "string",
  "permission_name": "string",
  "module_name": "string",
  "description": "string",
  "can_view": true,
  "can_add": true,
  "can_update": true,
  "can_delete": true,
  "can_export": true
}
```

---

## ═══════════════════════════════════════
## 5. PERMISSIONS  —  /api/Permissions
## ═══════════════════════════════════════

---

### POST /api/Permissions
Create a new permission.

**Request Body:**
```json
{
  "permission_code": "string",
  "permission_name": "string"
}
```

**Response `id`:** `guid` (new permission_id)

---

### GET /api/Permissions
Get all assignable permissions.

**Response `data`:** Array of Permission objects (same as role permissions response).

---

## ═══════════════════════════════════════
## 6. CLIENTS  —  /api/Clients
## ═══════════════════════════════════════
> A client IS a user. `client_id` = `user_id` of an existing user. Send the user_id from UI when creating.

---

### POST /api/Clients
Register an existing user as a client.

**Request Body:**
```json
{
  "client_id": "guid",
  "client_name": "string",
  "email": "string",
  "mobile": "string | null",
  "company_name": "string | null",
  "address": "string | null"
}
```

**Response `id`:** `guid` (client_id)

---

### PUT /api/Clients/{client_id}
Update client details.

**URL Param:** `client_id` (guid)

**Request Body:**
```json
{
  "client_name": "string | null",
  "email": "string | null",
  "mobile": "string | null",
  "company_name": "string | null",
  "address": "string | null"
}
```

**Response `id`:** `guid`

---

### GET /api/Clients/{client_id}
Get client by ID.

**URL Param:** `client_id` (guid)

**Response `data`:**
```json
{
  "client_id": "guid",
  "client_name": "string",
  "email": "string",
  "mobile": "string",
  "company_name": "string",
  "address": "string",
  "is_active": true,
  "created_at": "datetime",
  "project_count": 0
}
```

---

### GET /api/Clients
Get all clients (active + inactive) with project count.

**Response `data`:** Array of ClientDto.

---

### GET /api/Clients/active
Get only active clients with project count.

**Response `data`:** Array of ClientDto.

---

### PATCH /api/Clients/{client_id}/status?is_active={bool}
Activate or deactivate a client.

**URL Param:** `client_id` (guid)
**Query Param:** `is_active` (true/false)

**Response `data`:** `null`

---

## ═══════════════════════════════════════
## 7. PROJECTS  —  /api/Projects
## ═══════════════════════════════════════

---

### POST /api/Projects
Create a new project.

**Request Body:**
```json
{
  "client_id": "guid",
  "project_name": "string",
  "project_code": "string | null",
  "description": "string | null",
  "start_date": "date | null",
  "end_date": "date | null",
  "status": "NEW | IN_PROGRESS | ON_HOLD | COMPLETED | CANCELLED"
}
```

**Response `id`:** `guid` (new project_id)

---

### PUT /api/Projects/{project_id}
Update a project.

**URL Param:** `project_id` (guid)

**Request Body:**
```json
{
  "project_name": "string | null",
  "project_code": "string | null",
  "description": "string | null",
  "start_date": "date | null",
  "end_date": "date | null",
  "status": "NEW | IN_PROGRESS | ON_HOLD | COMPLETED | CANCELLED"
}
```

**Response `id`:** `guid`

---

### GET /api/Projects/{project_id}
Get project by ID.

**URL Param:** `project_id` (guid)

**Response `data`:**
```json
{
  "project_id": "guid",
  "client_id": "guid",
  "client_name": "string",
  "project_name": "string",
  "project_code": "string",
  "description": "string",
  "start_date": "date | null",
  "end_date": "date | null",
  "status": "NEW | IN_PROGRESS | ON_HOLD | COMPLETED | CANCELLED",
  "is_active": true,
  "created_at": "datetime"
}
```

---

### GET /api/Projects
Get all projects.

**Response `data`:** Array of ProjectDto.

---

### GET /api/Projects/by-client/{client_id}
Get all projects for a specific client.

**URL Param:** `client_id` (guid)

**Response `data`:** Array of ProjectDto.

---

### PATCH /api/Projects/{project_id}/status?is_active={bool}
Activate or deactivate a project.

**URL Param:** `project_id` (guid)
**Query Param:** `is_active` (true/false)

**Response `data`:** `null`

---

### PATCH /api/Projects/{project_id}/project-status
Update a project's lifecycle status.

**URL Param:** `project_id` (guid)

**Request Body:**
```json
{
  "status": "NEW | IN_PROGRESS | ON_HOLD | COMPLETED | CANCELLED"
}
```

**Response `data`:** `null`

---

## ═══════════════════════════════════════
## 8. TASKS  —  /api/Tasks
## ═══════════════════════════════════════
> Tasks are created under a `Project` and are bootstrapped from a `Workflow Template`. When a task is created, the template's steps are copied into `task_workflow_step` and become independent of the template.

---

### POST /api/Tasks
Create a new task (copies workflow template steps into task workflow steps in the same display order).

**Request Body:** `TaskCreateRequest`
```json
{
  "project_id": "guid",
  "workflow_template_id": "guid",
  "task_name": "string",
  "task_code": "string",
  "description": "string | null",
  "start_date": "date | null",
  "end_date": "date | null"
}
```

- Validations:
  - `project_id` must exist (404 if not found)
  - `workflow_template_id` must exist (404 if not found)
- Behavior: Create `task_master` and copy all steps from `workflow_template_step` -> `task_workflow_step` preserving `display_order` inside a single DB transaction.
- Response `id`: new `task_id`

---

### GET /api/Tasks
Get all tasks (summary list).

- Response `data`: `List<TaskDto>` (task header information without detailed step schedules)

---

### GET /api/Tasks/{task_id}
Get a task by id including its workflow steps ordered by `display_order`.

- Response `data`: `TaskDto` (includes `workflow_steps: List<TaskWorkflowStepDto>`)
- Errors: `404` if task not found

---

### PUT /api/Tasks/{task_id}
Update task metadata (partial updates supported).

**Request Body:** `TaskUpdateRequest`
```json
{
  "task_name": "string | null",
  "task_code": "string | null",
  "description": "string | null",
  "start_date": "date | null",
  "end_date": "date | null"
}
```

- Response `id`: `task_id`
- Errors: `404` if task not found

---

### DELETE /api/Tasks/{task_id}
Delete a task and its workflow steps. Deletion runs inside a DB transaction (steps deleted first).

- Response `id`: `task_id`
- Errors: `404` if task not found

---

### PUT /api/Tasks/{taskId}/workflow-steps/{taskWorkflowStepId}
Update schedule/notes for a single task workflow step.

**Request Body:** `TaskWorkflowStepUpdateRequest`
```json
{
  "start_date": "date | null",
  "end_date": "date | null",
  "notes": "string | null"
}
```

- Response `id`: `task_workflow_step_id`
- Errors: `404` if task or workflow step not found

---

### PUT /api/Tasks/{taskId}/workflow-steps/schedule
Batch update schedule/notes for multiple workflow steps of a task in a single transaction.

**Request Body:** `TaskWorkflowStepsScheduleRequest`
```json
{
  "steps": [
    {
      "task_workflow_step_id": "guid",
      "start_date": "date | null",
      "end_date": "date | null",
      "notes": "string | null"
    }
  ]
}
```

- Behavior: All updates applied in one DB transaction. If any referenced step is missing, the operation rolls back and returns `404`.
- Response: success message

---

## ═══════════════════════════════════════
## 9. WORKFLOW STEPS  —  /api/WorkflowSteps
## ═══════════════════════════════════════

---

### POST /api/WorkflowSteps
Create a new workflow step.

**Request Body:**
```json
{
  "workflow_step_name": "string",
  "workflow_step_code": "string",
  "description": "string | null"
}
```

**Response `id`:** `guid` (new workflow_step_id)

---

### PUT /api/WorkflowSteps/{step_id}
Update a workflow step.

**URL Param:** `step_id` (guid)

**Request Body:**
```json
{
  "workflow_step_name": "string | null",
  "description": "string | null"
}
```

**Response `id`:** `guid`

---

### GET /api/WorkflowSteps/{step_id}
Get workflow step by ID.

**URL Param:** `step_id` (guid)

**Response `data`:**
```json
{
  "workflow_step_id": "guid",
  "workflow_step_name": "string",
  "workflow_step_code": "string",
  "description": "string",
  "is_active": true,
  "created_at": "datetime"
}
```

---

### GET /api/WorkflowSteps
Get all workflow steps.

**Response `data`:** Array of WorkflowStepDto.

---

### GET /api/WorkflowSteps/active
Get only active workflow steps.

**Response `data`:** Array of WorkflowStepDto.

---

### PATCH /api/WorkflowSteps/{step_id}/status?is_active={bool}
Activate or deactivate a workflow step.

**URL Param:** `step_id` (guid)
**Query Param:** `is_active` (true/false)

**Response `data`:** `null`

---

## ═══════════════════════════════════════
## 9. WORKFLOW TEMPLATES  —  /api/WorkflowTemplates
## ═══════════════════════════════════════
> A template is a named, ordered collection of workflow steps (e.g. "Video Production" = Shoot → Edit → Approval → Publish)

---

### POST /api/WorkflowTemplates
Create a new workflow template with steps.

**Request Body:**
```json
{
  "workflow_template_name": "string",
  "workflow_template_code": "string",
  "description": "string | null",
  "steps": [
    {
      "workflow_step_id": "guid",
      "display_order": 1,
      "is_required": true
    }
  ]
}
```

**Response `id`:** `guid` (new workflow_template_id)

---

### PUT /api/WorkflowTemplates/{template_id}
Update template name/description only.

**URL Param:** `template_id` (guid)

**Request Body:**
```json
{
  "workflow_template_name": "string | null",
  "description": "string | null"
}
```

**Response `id`:** `guid`

---

### GET /api/WorkflowTemplates/{template_id}
Get template by ID including all its steps.

**URL Param:** `template_id` (guid)

**Response `data`:**
```json
{
  "workflow_template_id": "guid",
  "workflow_template_name": "string",
  "workflow_template_code": "string",
  "description": "string",
  "is_active": true,
  "created_at": "datetime",
  "steps": [
    {
      "workflow_template_step_id": "guid",
      "workflow_step_id": "guid",
      "workflow_step_name": "string",
      "workflow_step_code": "string",
      "description": "string",
      "display_order": 1,
      "is_required": true
    }
  ]
}
```

---

### GET /api/WorkflowTemplates
Get all workflow templates (without steps).

**Response `data`:** Array of WorkflowTemplateDto (steps array will be empty).

---

### PATCH /api/WorkflowTemplates/{template_id}/status?is_active={bool}
Activate or deactivate a workflow template.

**URL Param:** `template_id` (guid)
**Query Param:** `is_active` (true/false)

**Response `data`:** `null`

---

### PUT /api/WorkflowTemplates/{template_id}/steps
Replace all steps of a template (full replace, not merge).

**URL Param:** `template_id` (guid)

**Request Body:** Array of:
```json
[
  {
    "workflow_step_id": "guid",
    "display_order": 1,
    "is_required": true
  }
]
```

**Response `data`:** `null`

---

### GET /api/WorkflowTemplates/{template_id}/steps
Get all steps of a template.

**URL Param:** `template_id` (guid)

**Response `data`:** Array of WorkflowTemplateStepDto (same as steps inside GET by ID).

---

## ═══════════════════════════════════════
## ERROR RESPONSES
## ═══════════════════════════════════════

| statusCode | Meaning |
|---|---|
| 200 | Success |
| 400 | Bad request / validation error |
| 401 | Unauthorized — invalid or expired token |
| 404 | Record not found |
| 500 | Internal server error |

**Error response example:**
```json
{
  "statusCode": 404,
  "statusMessage": "404 Error",
  "errorMessage": "Client not found",
  "data": null,
  "id": null
}
```

---

## ═══════════════════════════════════════
## AUTHENTICATION NOTES
## ═══════════════════════════════════════

- After login, store `accessToken` and `refreshToken`
- Send `accessToken` in every protected request header:
  ```
  Authorization: Bearer <accessToken>
  ```
- When `accessToken` expires (401), call `POST /api/Auth/refresh` with `refreshToken` to get a new pair
- On logout, call `POST /api/Auth/logout` to revoke the `refreshToken`
- `MyProfile` endpoints identify the user from the JWT token — no user_id needed in URL

---

## ═══════════════════════════════════════
## BUSINESS RULES
## ═══════════════════════════════════════

1. **Client = User** — A client must already exist as a user. When creating a client, pass the `user_id` as `client_id`. The UI should first create the user, then register them as a client using the returned `user_id`.
2. **Workflow Template Steps** — `PUT /api/WorkflowTemplates/{id}/steps` is a full replace. Send the complete ordered list every time.
3. **Active/Inactive** — Use `PATCH /{id}/status?is_active=true/false` for all entities (clients, projects, workflow steps, workflow templates).
4. **Permissions** — `PUT /api/Roles/{id}/permissions` is a full replace. Send the complete permissions list every time.
5. **Profile Images** — Send `null`/empty file to remove the image. Allowed formats: jpg, jpeg, png, webp.
