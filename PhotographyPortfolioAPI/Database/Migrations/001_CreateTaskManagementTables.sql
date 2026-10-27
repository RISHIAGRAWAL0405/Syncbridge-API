-- SQL Migration for Task Management API
-- This script creates the task_master and task_workflow_step tables with status tracking

-- =====================================================
-- 1. CREATE task_master TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS task_master (
    task_id UUID PRIMARY KEY,
    project_id UUID NOT NULL,
    workflow_template_id UUID NOT NULL,
    task_name VARCHAR(255) NOT NULL,
    task_code VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP,
    CONSTRAINT fk_task_project FOREIGN KEY (project_id) REFERENCES project_master(project_id) ON DELETE CASCADE,
    CONSTRAINT fk_task_workflow_template FOREIGN KEY (workflow_template_id) REFERENCES workflow_template_master(workflow_template_id)
);

-- =====================================================
-- 2. CREATE task_workflow_step TABLE
-- =====================================================
CREATE TABLE IF NOT EXISTS task_workflow_step (
    task_workflow_step_id UUID PRIMARY KEY,
    task_id UUID NOT NULL,
    workflow_step_id UUID NOT NULL,
    step_name VARCHAR(255) NOT NULL,
    step_code VARCHAR(100) NOT NULL,
    display_order INTEGER NOT NULL,
    is_required BOOLEAN NOT NULL DEFAULT TRUE,
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    notes TEXT,
    status VARCHAR(30) NOT NULL DEFAULT 'NOT_STARTED',
    started_at TIMESTAMP,
    completed_at TIMESTAMP,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP,
    CONSTRAINT fk_step_task FOREIGN KEY (task_id) REFERENCES task_master(task_id) ON DELETE CASCADE,
    CONSTRAINT fk_step_workflow_step FOREIGN KEY (workflow_step_id) REFERENCES workflow_step_master(workflow_step_id),
    CONSTRAINT chk_step_status CHECK (status IN ('NOT_STARTED', 'IN_PROGRESS', 'COMPLETED', 'SKIPPED'))
);

-- =====================================================
-- 3. CREATE INDEXES for better query performance
-- =====================================================
CREATE INDEX IF NOT EXISTS idx_task_master_project_id ON task_master(project_id);
CREATE INDEX IF NOT EXISTS idx_task_master_workflow_template_id ON task_master(workflow_template_id);
CREATE INDEX IF NOT EXISTS idx_task_workflow_step_task_id ON task_workflow_step(task_id);
CREATE INDEX IF NOT EXISTS idx_task_workflow_step_status ON task_workflow_step(status);
CREATE INDEX IF NOT EXISTS idx_task_workflow_step_task_status ON task_workflow_step(task_id, status);

-- =====================================================
-- If you need to ALTER existing tables (if they exist without the new columns):
-- =====================================================
-- ALTER TABLE task_workflow_step ADD COLUMN IF NOT EXISTS status VARCHAR(30) NOT NULL DEFAULT 'NOT_STARTED';
-- ALTER TABLE task_workflow_step ADD COLUMN IF NOT EXISTS started_at TIMESTAMP;
-- ALTER TABLE task_workflow_step ADD COLUMN IF NOT EXISTS completed_at TIMESTAMP;
-- ALTER TABLE task_workflow_step ADD CONSTRAINT chk_step_status CHECK (status IN ('NOT_STARTED', 'IN_PROGRESS', 'COMPLETED', 'SKIPPED'));
