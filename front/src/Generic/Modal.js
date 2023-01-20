import { IconAlertTriangle } from "@tabler/icons";

export default function Modal({ id, title = null, text, confirm = null, cancel = null, onConfirm = () => {} }) {
  if (title === null) {
    title = "Are you sure?";
  }
  if (confirm === null) {
    confirm = "Confirm";
  }
  if (cancel === null) {
    cancel = "Cancel";
  }

  return (
    <div className="modal modal-blur fade" id={id} tabindex="-1" role="dialog" aria-hidden="true">
      <div className="modal-dialog modal-sm modal-dialog-centered" role="document">
        <div className="modal-content">
          <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          <div className="modal-status bg-danger"></div>
          <div className="modal-body text-center py-4">
            <IconAlertTriangle className="icon mb-2 text-danger icon-lg" />
            <h3>{title}</h3>
            <div className="text-muted">{text}</div>
          </div>
          <div className="modal-footer">
            <div className="w-100">
              <div className="row">
                <div className="col">
                  <button className="btn w-100" data-bs-dismiss="modal">
                    {cancel}
                  </button>
                </div>
                <div className="col">
                  <button className="btn btn-danger w-100" onClick={onConfirm} data-bs-dismiss="modal">
                    {confirm}
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
