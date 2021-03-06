require 'rubygems'
require 'albacore'
require 'configatron'

FRAMEWORK_PATH = "C:/Windows/Microsoft.NET/Framework/v4.0.30319/"
BUILD_PATH = File.expand_path('build')
MERGE_PATH = "#{BUILD_PATH}/merge"
TOOLS_PATH = File.expand_path('tools')
ARTIFACTS_PATH = File.expand_path('artifacts')
LIB_PATH = File.expand_path('lib')
SOLUTION = "src/Chunks.sln"
COMPILE_TARGET = "Release"
FEEDS = ["https://go.microsoft.com/fwlink/?LinkID=206669" ]
load "VERSION.txt"
require './packages.rb'

task :default => ["build:all"]

namespace :build do

	task :all => [:clean, :dependencies, :compile, :tests, :package]

	assemblyinfo :versioning do |asm|
  		asm.output_file = "src/CommonAssemblyInfo.cs"
  		asm.version = "#{BUILD_VERSION}"
	end

	task :clean do
		rm_rf "#{BUILD_PATH}"
		rm_rf "#{ARTIFACTS_PATH}"
	end

	task :compile => [:versioning] do

		mkdir "#{BUILD_PATH}"
		sh "#{FRAMEWORK_PATH}msbuild.exe /p:Configuration=#{COMPILE_TARGET} #{SOLUTION}"
		copyOutputFiles "src/Chunks/bin/#{COMPILE_TARGET}", "*.{exe,dll,pdb}", "#{BUILD_PATH}"
	end

	task :tests do
		specs = FileList.new("src/Chunks.Specs/bin/#{COMPILE_TARGET}/*.Specs.dll")
		sh "lib/Machine.Specifications/tools/mspec-x86-clr4.exe -x integration #{specs}"
	end

	task :package do
		mkdir_p "#{ARTIFACTS_PATH}"
		rm Dir.glob("#{ARTIFACTS_PATH}/*.nupkg")
		FileList["packaging/nuget/*.nuspec"].each do |spec|
		sh "\"#{TOOLS_PATH}/nuget/NuGet.exe\" pack #{spec} -o \"#{ARTIFACTS_PATH}\" -Version #{BUILD_VERSION} -BasePath . -Symbols"
		end
	end

	task :dependencies do
		feeds = FEEDS.map {|x|"-Source " + x }.join(' ')
		configatron.packages.each do | name,version |
			feeds = feeds unless !version
			packageExists = File.directory?("#{LIB_PATH}/#{name}")
			versionInfo="#{LIB_PATH}/#{name}/version.info"
			currentVersion=IO.read(versionInfo) if File.exists?(versionInfo)
			if(!packageExists or !version or !versionInfo or currentVersion != version) then
				versionArg = "-Version #{version}" unless !version
				sh "nuget Install #{name} #{versionArg} -o #{LIB_PATH} #{feeds} -ExcludeVersion" do | ok, results |
					File.open(versionInfo, 'w') {|f| f.write(version) } unless !ok
				end
			end
		end
    end


	def copyOutputFiles(fromDir, filePattern, outDir)
		Dir.glob(File.join(fromDir, filePattern)){|file| 		
			copy(file, outDir) if File.file?(file)
  		} 
	end
end
