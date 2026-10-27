-- =====================================================
-- Migration 004: Task Workflow Step Links
-- Stores video/image/file links attached to workflow steps
-- category = step name (e.g. "Approval") drives client review UI
-- Each link has an approval status: PENDING | APPROVED | REJECTED
-- =====================================================

CREATE TABLE IF NOT EXISTS task_workflow_step_link (
    link_id               UUID          PRIMARY KEY,
    task_workflow_step_id UUID          NOT NULL,
    task_id               UUID          NOT NULL,
    category              VARCHAR(100)  NOT NULL DEFAULT '',  -- step name; "Approval" enables client approve/reject
    title                 VARCHAR(255)  NOT NULL,
    url                   TEXT          NOT NULL,
    link_type             VARCHAR(50)   NOT NULL DEFAULT 'VIDEO',   -- VIDEO | IMAGE | FILE | OTHER
    description           TEXT,
    status                VARCHAR(20)   NOT NULL DEFAULT 'PENDING', -- PENDING | APPROVED | REJECTED
    status_note           TEXT,
    uploaded_by           UUID,
    reviewed_by           UUID,
    reviewed_at           TIMESTAMPTZ,
    is_active             BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at            TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at            TIMESTAMPTZ,
    CONSTRAINT fk_step_link_step FOREIGN KEY (task_workflow_step_id)
        REFERENCES task_workflow_step(task_workflow_step_id) ON DELETE CASCADE,
    CONSTRAINT fk_step_link_task FOREIGN KEY (task_id)
        REFERENCES task_master(task_id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_step_link_step_id ON task_workflow_step_link(task_workflow_step_id);
CREATE INDEX IF NOT EXISTS idx_step_link_task_id ON task_workflow_step_link(task_id);
CREATE INDEX IF NOT EXISTS idx_step_link_status  ON task_workflow_step_link(status);
CREATE INDEX IF NOT EXISTS idx_step_link_category ON task_workflow_step_link(category);

-- If table already exists, add the category column:
-- ALTER TABLE task_workflow_step_link ADD COLUMN IF NOT EXISTS category VARCHAR(100) NOT NULL DEFAULT '';
