# ThemeDev

## Jenkins Build Command
xvfb-run --server-args="-screen 0 1024x768x24" /opt/Unity/Editor/Unity -batchmode -logfile -force-opengl - quit -projectPath /var/lib/jenkins/workspace/ThemeDev/ -executeMethod BuildOps.PerformWindowsBuild
